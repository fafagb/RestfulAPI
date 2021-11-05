using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using API.Extensions;
using API.Helpers;
using API.Core.Entities;
using API.Core.Interfaces;
using API.Repository.Database;
using API.Repository.Extensions;
using API.Repository.Resources;
using API.Repository.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace API.Controllers
{
    //路由地址定好后尽量不改
    [Route("api/persons")]
    public class PersonController : Controller
    {
        //注入IPersonRepository和IUnitOfWork是同一个DbContext,生命周期都是scope
        private readonly IPersonRepository _personRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUrlHelper _urlHelper;
        private readonly ITypeHelperService _typeHelperService;
        private readonly IPropertyMappingContainer _propertyMappingContainer;

        public PersonController(
            IPersonRepository personRepository,
            IUnitOfWork unitOfWork,
            IConfiguration configuration,
           ILoggerFactory logger,
           IMapper mapper, IUrlHelper urlHelper,
           ITypeHelperService typeHelperService,
           IPropertyMappingContainer propertyMappingContainer)
        {
            _personRepository = personRepository;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _logger = logger.CreateLogger("API.Controllers.PersonController");
            _mapper = mapper;
            _urlHelper = urlHelper;
            _typeHelperService = typeHelperService;
            _propertyMappingContainer = propertyMappingContainer;
        }

        #region private method

        private IEnumerable<LinkResource> CreateLinksForPerson(int id, string fields = null)
        {
            var links = new List<LinkResource>();
            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(new LinkResource(_urlHelper.Link("GetPerson", new { id }), "self", "GET"));
            }
            else
            {
                links.Add(new LinkResource(_urlHelper.Link("GetPerson", new { id, fields }), "self", "GET"));

            }
            links.Add(new LinkResource(_urlHelper.Link("DeletePerson", new { id, fields }), "delete_person", "DELETE"));

            return links;

        }

        private IEnumerable<LinkResource> CreateLinksForPerson(PersonParameters personParameters, bool hasPrevious, bool hasNext)
        {
            var links = new List<LinkResource> {
                new LinkResource(
                    CreatePersonUri(personParameters,PaginationResourceUriType.CurrentPage),"self","GET")
            };

            if (hasPrevious)
            {
                links.Add(new LinkResource(
                     CreatePersonUri(personParameters, PaginationResourceUriType.PreviousPage), "previous_page", "GET"));
            };
            if (hasNext)
            {
                links.Add(new LinkResource(
                     CreatePersonUri(personParameters, PaginationResourceUriType.NextPage), "next_page", "GET"));
            };

            return links;
        }




        private string CreatePersonUri(PersonParameters parameters, PaginationResourceUriType uriType)
        {
            switch (uriType)
            {
                case PaginationResourceUriType.PreviousPage:
                    var previousParameters = new
                    {
                        pageIndex = parameters.PageIndex - 1,
                        pageSize = parameters.PageSize,
                        orderBy = parameters.OrderBy,
                        fields = parameters.Fields
                    };
                    return _urlHelper.Link("GetPersons", previousParameters);
                case PaginationResourceUriType.NextPage:
                    var nextParameters = new
                    {
                        pageIndex = parameters.PageIndex + 1,
                        pageSize = parameters.PageSize,
                        orderBy = parameters.OrderBy,
                        fields = parameters.Fields
                    };
                    return _urlHelper.Link("GetPersons", nextParameters);
                default:
                    var currentParameters = new
                    {
                        pageIndex = parameters.PageIndex,
                        pageSize = parameters.PageSize,
                        orderBy = parameters.OrderBy,
                        fields = parameters.Fields
                    };
                    return _urlHelper.Link("GetPersons", currentParameters);
            }
        }
        #endregion




        #region public action
        [HttpGet(Name = "GetPersons")]
        [RequestHeaderMatchingMediaTypeAttribute("Accept", new[] { "application/vnd..hateoas+json" })]
        public async Task<IActionResult> GetHateoas(PersonParameters personParameters, [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!_propertyMappingContainer.ValidateMappingExsitsFor<PersonResource, Person>(personParameters.OrderBy))
            {
                return BadRequest("can't find fields for  sorting");
            }
            if (!_typeHelperService.TypeHasProperties<PersonResource>(personParameters.Fields))
            {
                return BadRequest("Fields not exist");
            }

            var personList = await _personRepository.GetAllPersonAsync(personParameters);
            var personResources = _mapper.Map<IEnumerable<Person>, IEnumerable<PersonResource>>(personList);


            var shapedPersonResources = personResources.ToDynamicIEnumerable(personParameters.Fields);

            var shapedWithLinks = shapedPersonResources.Select(x =>
            {
                var dict = x as IDictionary<String, object>;
                var personlinks = CreateLinksForPerson((int)dict["Id"], personParameters.Fields);
                dict.Add("links", personlinks);
                return dict;
            });


            var links = CreateLinksForPerson(personParameters, personList.HasPrevious, personList.HasNext);

            var result = new
            {
                value = shapedWithLinks,
                links

            };



            var mata = new
            {
                personList.PageSize,
                personList.PageIndex,
                personList.TotalItemsCount,
                personList.PageCount,
                //previousPageLink,
                //nextPageLink
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(mata, new JsonSerializerSettings()
            {

                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));

            return Ok(result);

        }





        [HttpGet(Name = "GetPersons")]
        [RequestHeaderMatchingMediaTypeAttribute("Accept", new[] { "application/json" })]
        public async Task<IActionResult> Get(PersonParameters personParameters, [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!_propertyMappingContainer.ValidateMappingExsitsFor<PersonResource, Person>(personParameters.OrderBy))
            {
                return BadRequest("can't find fields for  sorting");
            }
            if (!_typeHelperService.TypeHasProperties<PersonResource>(personParameters.Fields))
            {
                return BadRequest("Fields not exist");
            }

            var personList = await _personRepository.GetAllPersonAsync(personParameters);
            var personResources = _mapper.Map<IEnumerable<Person>, IEnumerable<PersonResource>>(personList);

            var previousPageLink = personList.HasPrevious ? CreatePersonUri(personParameters, PaginationResourceUriType.PreviousPage) : null;
            var nextPageLink = personList.HasNext ? CreatePersonUri(personParameters, PaginationResourceUriType.NextPage) : null;
            var mata = new
            {
                personList.PageSize,
                personList.PageIndex,
                personList.TotalItemsCount,
                personList.PageCount,
                previousPageLink,
                nextPageLink
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(mata, new JsonSerializerSettings()
            {

                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));

            return Ok(personResources.ToDynamicIEnumerable(personParameters.Fields));
        }









        [HttpGet("{id}", Name = "GetPerson")]
        public async Task<IActionResult> Get(int id, string fields = null)
        {

            if (!_typeHelperService.TypeHasProperties<PersonResource>(fields))
            {
                return BadRequest("Fields not exist");
            }
            var person = await _personRepository.GetPersonByIdAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            var personResources = _mapper.Map<Person, PersonResource>(person);

            var shapedPersonResources = personResources.ToDynamic(fields);
            var links = CreateLinksForPerson(id, fields);

            var result = shapedPersonResources as IDictionary<String, object>;
            result.Add("links", links);
            return Ok(result);
        }








        [HttpPost(Name = "CreatePerson")]
        [RequestHeaderMatchingMediaTypeAttribute("Content-Type", new[] { "application/vnd..post.create+json" })]
        [RequestHeaderMatchingMediaTypeAttribute("Accept", new[] { "application/vnd..hateoas+json" })]
        public async Task<IActionResult> Person([FromBody] PersonAddResource personAddResource)
        {
            if (personAddResource == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return new MyUnprocessableEntityObjectResult(ModelState);
            }
            var newPerson = _mapper.Map<PersonAddResource, Person>(personAddResource);
            newPerson.UpdateTime = DateTime.Now;
            _personRepository.AddPerson(newPerson);
            if (!await _unitOfWork.SaveAsync())
            {
                throw new Exception("Save Failed!");

            }
            var resultResource = _mapper.Map<Person, PersonResource>(newPerson);

            var links = CreateLinksForPerson(Convert.ToInt32(newPerson.Id));

            var linkPersonResource = resultResource.ToDynamic() as IDictionary<string, object>;
            linkPersonResource.Add("links", links);


            return CreatedAtRoute("GetPerson", new { id = linkPersonResource["Id"] }, linkPersonResource);
        }


        [HttpDelete("{id}", Name = "DeletePerson")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var person = await _personRepository.GetPersonByIdAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            _personRepository.Delete(person);
            if (!await _unitOfWork.SaveAsync())
            {
                throw new Exception($"Deleting  person{id} failed when saving.");
            }
            return NoContent();
        }



        [HttpPut("{id}", Name = "UpdatePerson")]
        [RequestHeaderMatchingMediaType("Content-Type", new[] { "application/vnd..put.update+json" })]
        public async Task<IActionResult> UpdatePerson(int id, [FromBody] PersonUpdateResource personUpdate)
        {
            if (personUpdate == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return new MyUnprocessableEntityObjectResult(ModelState);
            }
            var person = await _personRepository.GetPersonByIdAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            person.UpdateTime = DateTime.Now;
            _mapper.Map(personUpdate, person);
            if (!await _unitOfWork.SaveAsync())
            {
                throw new Exception($"Updating person {id} failed when saving");
            }
            return NoContent();
        }
        
        [HttpPatch("{id}", Name = "PartiallyUpdatePerson")]
        public async Task<IActionResult> PartiallyUpdatePerson(int id, [FromBody] JsonPatchDocument<PersonUpdateResource> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }
            var person = await _personRepository.GetPersonByIdAsync(id);

            if (person == null)
            {
                return NotFound();
            }
            var personToPatch = _mapper.Map<PersonUpdateResource>(person);
            patchDoc.ApplyTo(personToPatch, (Microsoft.AspNetCore.JsonPatch.Adapters.IObjectAdapter)ModelState);


            TryValidateModel(personToPatch);
            if (!ModelState.IsValid)
            {
                return new MyUnprocessableEntityObjectResult(ModelState);
            }

            _mapper.Map(personToPatch, person);
            person.UpdateTime = DateTime.Now;
            _personRepository.Update(person);
            try
            {
                if (!await _unitOfWork.SaveAsync())
                {
                    throw new Exception($"Patching person {id} failed when saving");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
            return NoContent();
        }


        #endregion





    }
}
