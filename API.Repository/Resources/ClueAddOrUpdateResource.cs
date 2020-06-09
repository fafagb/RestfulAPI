using System;
using System.Collections.Generic;
using System.Text;

namespace API.Repository.Resources
{
   public  class ClueAddOrUpdateResource
    {
        public int Id { get; set; }
        public int Agentid { get; set; }
        public int Smsid { get; set; }
        public string Licenseno { get; set; }
        public int Source { get; set; }
        public string Sourcename { get; set; }
        public int Casetype { get; set; }
        public int Followupstate { get; set; }
        public string Accidentremark { get; set; }
        public string Smsrecivedtime { get; set; }
        public string Dangerarea { get; set; }
        public string Mobile { get; set; }
        public string ReportCaseNum { get; set; }
        public string ReportCasePeople { get; set; }
        public int? HasInsureInfo { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public int Deleted { get; set; }
        public string MoldName { get; set; }
        public string CarVin { get; set; }
        public string CityName { get; set; }
        public int LastFollowId { get; set; }
        public string OrderNum { get; set; }
        public int CurrentAgentId { get; set; }
        public string AgentName { get; set; }
        public int ChosedModelId { get; set; }
        public int ClueFromType { get; set; }
        public int ChosedCompanyId { get; set; }
        public string ChosedCompanyName { get; set; }
        public string CarOwner { get; set; }
        public int IsMany { get; set; }
        public int IsDrivering { get; set; }
        public int Only4s { get; set; }
        public string ReceiveCarAddress { get; set; }
        public double ReceiveLng { get; set; }
        public double ReceiveLat { get; set; }
        public string ExpectedAddress { get; set; }
        public double ExpectedLng { get; set; }
        public double ExpectedLat { get; set; }
        public int Province { get; set; }
        public int City { get; set; }
        public int Area { get; set; }
        public string ProvinceName { get; set; }
        public string CityName1 { get; set; }
        public string AreaName { get; set; }
        public string FromAgentName { get; set; }
        public int ToAgentId { get; set; }
        public string ToAgentName { get; set; }
        public decimal FromRate { get; set; }
        public decimal ToRate { get; set; }
        public decimal MaintainAmount { get; set; }
        public DateTime AcceptedTime { get; set; }
        public string ChosedModelName { get; set; }
        public int AcceptedState { get; set; }
        public int ToSettledState { get; set; }
        public int FromSettledState { get; set; }
        public DateTime ToSettledTime { get; set; }
        public DateTime FromSettledTime { get; set; }
        public decimal Profits { get; set; }
        public DateTime ExpectedFinishedTime { get; set; }
        public int AgentType { get; set; }

    }
}
