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
    [Route("api/clues")]
    public class ClueController : Controller
    {
        //注入IClueRepository和IUnitOfWork是同一个DbContext,生命周期都是scope
        private readonly IClueRepository _clueRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUrlHelper _urlHelper;
        private readonly ITypeHelperService _typeHelperService;
        private readonly IPropertyMappingContainer _propertyMappingContainer;

        public ClueController(
            IClueRepository clueRepository,
            IUnitOfWork unitOfWork,
            IConfiguration configuration,
           ILoggerFactory logger,
           IMapper mapper, IUrlHelper urlHelper,
           ITypeHelperService typeHelperService,
           IPropertyMappingContainer propertyMappingContainer)
        {
            _clueRepository = clueRepository;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _logger = logger.CreateLogger("API.Controllers.ClueController");
            _mapper = mapper;
            _urlHelper = urlHelper;
            _typeHelperService = typeHelperService;
            _propertyMappingContainer = propertyMappingContainer;
        }

        #region private method

        private IEnumerable<LinkResource> CreateLinksForClue(int id, string fields = null)
        {
            var links = new List<LinkResource>();
            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(new LinkResource(_urlHelper.Link("GetClue", new { id }), "self", "GET"));
            }
            else
            {
                links.Add(new LinkResource(_urlHelper.Link("GetClue", new { id, fields }), "self", "GET"));

            }
            links.Add(new LinkResource(_urlHelper.Link("DeleteClue", new { id, fields }), "delete_clue", "DELETE"));

            return links;

        }

        private IEnumerable<LinkResource> CreateLinksForClues(ClueParameters clueParameters, bool hasPrevious, bool hasNext)
        {
            var links = new List<LinkResource> {
                new LinkResource(
                    CreateClueUri(clueParameters,PaginationResourceUriType.CurrentPage),"self","GET")
            };

            if (hasPrevious)
            {
                links.Add(new LinkResource(
                     CreateClueUri(clueParameters, PaginationResourceUriType.PreviousPage), "previous_page", "GET"));
            };
            if (hasNext)
            {
                links.Add(new LinkResource(
                     CreateClueUri(clueParameters, PaginationResourceUriType.NextPage), "next_page", "GET"));
            };

            return links;
        }




        private string CreateClueUri(ClueParameters parameters, PaginationResourceUriType uriType)
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
                    return _urlHelper.Link("GetClues", previousParameters);
                case PaginationResourceUriType.NextPage:
                    var nextParameters = new
                    {
                        pageIndex = parameters.PageIndex + 1,
                        pageSize = parameters.PageSize,
                        orderBy = parameters.OrderBy,
                        fields = parameters.Fields
                    };
                    return _urlHelper.Link("GetClues", nextParameters);
                default:
                    var currentParameters = new
                    {
                        pageIndex = parameters.PageIndex,
                        pageSize = parameters.PageSize,
                        orderBy = parameters.OrderBy,
                        fields = parameters.Fields
                    };
                    return _urlHelper.Link("GetClues", currentParameters);
            }
        }
        #endregion




        #region public action
        [HttpGet(Name = "GetClues")]
        [RequestHeaderMatchingMediaTypeAttribute("Accept", new[] { "application/vnd..hateoas+json" })]
        public async Task<IActionResult> GetHateoas(ClueParameters clueParameters, [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!_propertyMappingContainer.ValidateMappingExsitsFor<ClueResource, TxClues>(clueParameters.OrderBy))
            {
                return BadRequest("can't find fields for  sorting");
            }
            if (!_typeHelperService.TypeHasProperties<ClueResource>(clueParameters.Fields))
            {
                return BadRequest("Fields not exist");
            }

            var clueList = await _clueRepository.GetAllCluesAsync(clueParameters);
            var clueResources = _mapper.Map<IEnumerable<TxClues>, IEnumerable<ClueResource>>(clueList);


            var shapedClueResources = clueResources.ToDynamicIEnumerable(clueParameters.Fields);

            var shapedWithLinks = shapedClueResources.Select(x =>
            {
                var dict = x as IDictionary<String, object>;
                var cluelinks = CreateLinksForClue((int)dict["Id"], clueParameters.Fields);
                dict.Add("links", cluelinks);
                return dict;
            });


            var links = CreateLinksForClues(clueParameters, clueList.HasPrevious, clueList.HasNext);

            var result = new
            {
                value = shapedWithLinks,
                links

            };



            var mata = new
            {
                clueList.PageSize,
                clueList.PageIndex,
                clueList.TotalItemsCount,
                clueList.PageCount,
                //previousPageLink,
                //nextPageLink
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(mata, new JsonSerializerSettings()
            {

                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));

            return Ok(result);

        }





        [HttpGet(Name = "GetClues")]
        [RequestHeaderMatchingMediaTypeAttribute("Accept", new[] { "application/json" })]
        public async Task<IActionResult> Get(ClueParameters clueParameters, [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!_propertyMappingContainer.ValidateMappingExsitsFor<ClueResource, TxClues>(clueParameters.OrderBy))
            {
                return BadRequest("can't find fields for  sorting");
            }
            if (!_typeHelperService.TypeHasProperties<ClueResource>(clueParameters.Fields))
            {
                return BadRequest("Fields not exist");
            }

            var clueList = await _clueRepository.GetAllCluesAsync(clueParameters);
            var clueResources = _mapper.Map<IEnumerable<TxClues>, IEnumerable<ClueResource>>(clueList);

            var previousPageLink = clueList.HasPrevious ? CreateClueUri(clueParameters, PaginationResourceUriType.PreviousPage) : null;
            var nextPageLink = clueList.HasNext ? CreateClueUri(clueParameters, PaginationResourceUriType.NextPage) : null;
            var mata = new
            {
                clueList.PageSize,
                clueList.PageIndex,
                clueList.TotalItemsCount,
                clueList.PageCount,
                previousPageLink,
                nextPageLink
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(mata, new JsonSerializerSettings()
            {

                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));

            return Ok(clueResources.ToDynamicIEnumerable(clueParameters.Fields));
        }









        [HttpGet("{id}", Name = "GetClue")]
        public async Task<IActionResult> Get(int id, string fields = null)
        {

            if (!_typeHelperService.TypeHasProperties<ClueResource>(fields))
            {
                return BadRequest("Fields not exist");
            }
            var clues = await _clueRepository.GetClueByIdAsync(id);
            if (clues == null)
            {
                return NotFound();
            }
            var clueResources = _mapper.Map<TxClues, ClueResource>(clues);

            var shapedClueResources = clueResources.ToDynamic(fields);
            var links = CreateLinksForClue(id, fields);

            var result = shapedClueResources as IDictionary<String, object>;
            result.Add("links", links);
            return Ok(result);
        }








        [HttpPost(Name = "CreateClue")]
        [RequestHeaderMatchingMediaTypeAttribute("Content-Type", new[] { "application/vnd..post.create+json" })]
        [RequestHeaderMatchingMediaTypeAttribute("Accept", new[] { "application/vnd..hateoas+json" })]
        public async Task<IActionResult> Clue([FromBody] ClueAddResource clueAddResource)
        {
            if (clueAddResource == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return new MyUnprocessableEntityObjectResult(ModelState);
            }
            var newClue = _mapper.Map<ClueAddResource, TxClues>(clueAddResource);
            newClue.UpdateTime = DateTime.Now;
            _clueRepository.AddClue(newClue);
            if (!await _unitOfWork.SaveAsync())
            {
                throw new Exception("Save Failed!");

            }
            var resultResource = _mapper.Map<TxClues, ClueResource>(newClue);

            var links = CreateLinksForClue(newClue.Id);

            var linkClueResource = resultResource.ToDynamic() as IDictionary<string, object>;
            linkClueResource.Add("links", links);


            return CreatedAtRoute("GetClue", new { id = linkClueResource["Id"] }, linkClueResource);
        }


        [HttpDelete("{id}", Name = "DeleteClue")]
        public async Task<IActionResult> DeleteClue(int id)
        {
            var clue = await _clueRepository.GetClueByIdAsync(id);
            if (clue == null)
            {
                return NotFound();
            }
            _clueRepository.Delete(clue);
            if (!await _unitOfWork.SaveAsync())
            {
                throw new Exception($"Deleting  clue{id} failed when saving.");
            }
            return NoContent();
        }



        [HttpPut("{id}", Name = "UpdateClue")]
        [RequestHeaderMatchingMediaType("Content-Type", new[] { "application/vnd..put.update+json" })]
        public async Task<IActionResult> UpdateClue(int id, [FromBody] ClueUpdateResource clueUpdate)
        {
            if (clueUpdate == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return new MyUnprocessableEntityObjectResult(ModelState);
            }
            var clue = await _clueRepository.GetClueByIdAsync(id);
            if (clue == null)
            {
                return NotFound();
            }
            clue.UpdateTime = DateTime.Now;
            _mapper.Map(clueUpdate, clue);
            if (!await _unitOfWork.SaveAsync())
            {
                throw new Exception($"Updating clue {id} failed when saving");
            }
            return NoContent();
        }
        
        [HttpPatch("{id}", Name = "PartiallyUpdateClue")]
        public async Task<IActionResult> PartiallyUpdateClue(int id, [FromBody] JsonPatchDocument<ClueUpdateResource> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }
            var clue = await _clueRepository.GetClueByIdAsync(id);

            if (clue == null)
            {
                return NotFound();
            }
            var clueToPatch = _mapper.Map<ClueUpdateResource>(clue);
            patchDoc.ApplyTo(clueToPatch, ModelState);


            TryValidateModel(clueToPatch);
            if (!ModelState.IsValid)
            {
                return new MyUnprocessableEntityObjectResult(ModelState);
            }

            _mapper.Map(clueToPatch, clue);
            clue.UpdateTime = DateTime.Now;
            _clueRepository.Update(clue);
            try
            {
                if (!await _unitOfWork.SaveAsync())
                {
                    throw new Exception($"Patching clue {id} failed when saving");
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
