using Core.Entities;
using Core.Entities.Enum;
using Repository.Extensions.Utility;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
namespace Repository.Extensions
{
    public static class VisitRepoExtensions
    {
        public static IQueryable<Visit> Search(this IQueryable<Visit> visits, string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return visits;
            var searchLower = searchTerm.Trim().ToLower();
            return visits.Where(d =>
                                      d.EmployeeNameInVisit.ToLower().Contains(searchLower) ||
                                      d.Visitors.Any(v => v.Name.ToLower().Contains(searchLower)) ||
                                      d.Visitors.Any(v => v.NID.ToLower().Contains(searchLower)) ||
                                      d.Visitors.Any(v => v.Phone.ToLower().Contains(searchLower)) ||
                                      d.Reason.ToLower().Contains(searchLower) ||
                                      d.Notes.ToLower().Contains(searchLower));
        }
        public static IQueryable<Visit> filter(this IQueryable<Visit> visits, long? floorId, VisitState? visitStateFromPolice, VisitState? visitStateFromDept, VisitType? visitType,bool? isCreatedByDept, long? CardId)
        {
            if (floorId != null)
            {
                visits = visits.Where(v => v.FloorId.Equals(floorId));
            }
            if(isCreatedByDept != null)
            {
                visits = visits.Where(v => v.IsCreatedByDept == isCreatedByDept);
            }
            if (CardId != null)
            {
                visits = visits.Where(v => v.Visitors.Any(v => v.CardId == CardId));
            }
            switch (visitStateFromPolice)
            {
                case VisitState.Approved:
                    visits = visits.Where(v => v.VisitStateFromPolice == VisitState.Approved);
                    break;
                case VisitState.Pending:
                    visits = visits.Where(v => v.VisitStateFromPolice == VisitState.Pending);
                    break;
                case VisitState.Rejected:
                    visits = visits.Where(v => v.VisitStateFromPolice == VisitState.Rejected);
                    break;
                default:
                    break;
            }
            switch (visitStateFromDept)
            {
                case VisitState.Approved:
                    visits = visits.Where(v => v.VisitStateFromDept == VisitState.Approved);
                    break;
                case VisitState.Pending:
                    visits = visits.Where(v => v.VisitStateFromDept == VisitState.Pending);
                    break;
                case VisitState.Rejected:
                    visits = visits.Where(v => v.VisitStateFromDept == VisitState.Rejected);
                    break;
                default:
                    break;
            }
            switch (visitType)
            {
                case VisitType.Employee:
                    visits = visits.Where(v => v.VisitType == VisitType.Employee);
                    break;
                case VisitType.External:
                    visits = visits.Where(v => v.VisitType == VisitType.External);
                    break;
                default:
                    break;
            }
            return visits;
        }
        public static IQueryable<Visit> Sort(this IQueryable<Visit> visits, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return visits.OrderBy(e => e.VisitDate);

            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(Visit).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();
            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;
                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(pi =>
               pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
                if (objectProperty == null)
                    continue;
                var direction = param.EndsWith(" desc") ? "descending" : "ascending";
                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction},");
            }
            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
            if (string.IsNullOrWhiteSpace(orderQuery))
                return visits.OrderBy(e => e.VisitDate);
            return visits.OrderBy(orderQuery);
        }


    }

    public static class BlackListRepoExtensions
    {
        public static IQueryable<VisitorBlackList> Search(this IQueryable<VisitorBlackList> visits, string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return visits;
            var searchLower = searchTerm.Trim().ToLower();
            return visits.Where(d =>d.VisitorIdentifierNIDorPassportNumber.ToLower().Contains(searchLower)  );
        }
    }
}
