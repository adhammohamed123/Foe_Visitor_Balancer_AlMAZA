namespace Core.Features
{
    public class PagedList<T> : List<T>
	{
		public MetaData metaData { get; set; }
		public PagedList(IEnumerable<T> source, int totalRecords, int pageNumber, int pageSize)
		{
			metaData = new MetaData()
			{
				CurrentPage = pageNumber,
				PageSize = pageSize,
				TotalRecords = totalRecords,
				TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
			};
			AddRange(source);
		}
		public static PagedList<T> ToPagedList(IEnumerable<T> source, int pageNumber, int pageSize)
		{
			var totalRecords = source.Count();
			var items = source.
				Skip((pageNumber - 1) * pageSize)
				.Take(pageSize).ToList();

			return new PagedList<T>(items, totalRecords, pageNumber, pageSize);
		}
       

    }
}
