using Core.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Features
{
	public abstract class RequestParameters
	{
		public int PageNumber { get; set; } = 1;
		public int PageSize
		{
			get { return _pageSize; }
			set { _pageSize = (value > maxPageSize) ? maxPageSize : value; }
		}

		const int maxPageSize = 50;
		private int _pageSize = 10;
    }
	public class VisitRequestParameters : RequestParameters
	{
        public bool? IsCreatedByDept { get; set; }
        public string? OrderBy { get; set; }
        public VisitRequestParameters()=> OrderBy = "VisitDate desc";

        public VisitState? VisitStateFromPolice { get; set; }
        public VisitState? VisitStateFromDept { get; set; }

        public VisitType? VisitType { get; set; }
        public long? FloarId { get; set; }
        public string? SearchTerm { get; set; }
    }
	public class VisitorRequestParameters : RequestParameters
	{
		public string? SearchTerm { get; set; }
    }

    public class VisitorBlackListRequestParameters : RequestParameters
    {
        public string? SearchTerm { get; set; }
    }
    public class DepartmentRequestParameters : RequestParameters
    {
        public string? SearchTerm { get; set; }
    }
    public class UsersRequestParameters : RequestParameters
    {
        public string? SearchTerm { get; set; }
    }
  
}
