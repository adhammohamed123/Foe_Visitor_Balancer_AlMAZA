using Core.Entities;
using Core.RepositoriesContracts;
using   Core.RepositoryContracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Repositories;
using System.Diagnostics;

namespace Repository
{


    public class RepositoryManager:IRepositoryManager
    {
        private readonly FoeVisitContext context;


        private Lazy<IUserRepo> _userRepo;
        private Lazy<ICardRepo> _CardtRepo;
        private Lazy<IFloorRepo> _FloorRepo;
        private Lazy<IVisitRepo> _VisitRepo;
        private Lazy<IVisitorRepo> _VisitorRepo;
        private Lazy<IDepartmentRepo> _DepartmentRepo;
        private Lazy<IVisitorBlackListRepo> _BlackListRepo;
		private Lazy<IStorageService> _storageService;
		public RepositoryManager(FoeVisitContext context,IHostingEnvironment hostingEnvironment)
        {
            this.context = context;
            _userRepo = new Lazy<IUserRepo>(() => new UserRepo(context));
            _CardtRepo = new Lazy<ICardRepo>(() => new CardRepo(context));
            _FloorRepo= new Lazy<IFloorRepo>(() => new FloorRepo(context));
            _VisitorRepo= new Lazy<IVisitorRepo>(() => new VisitorRepo(context));
            _VisitRepo= new Lazy<IVisitRepo>(() => new VisitRepo(context));
            _DepartmentRepo = new Lazy<IDepartmentRepo>(() => new DepartmentRepo(context));
			_BlackListRepo = new Lazy<IVisitorBlackListRepo>(() => new VisitorBlackListRepo(context));
			_storageService = new Lazy<IStorageService>(() => new FileStorageService(hostingEnvironment));
		}

        public IUserRepo UserRepo =>_userRepo.Value;

        public ICardRepo CardRepo => _CardtRepo.Value;

        public IFloorRepo FloorRepo => _FloorRepo.Value;

        public IVisitRepo VisitRepo => _VisitRepo.Value;

        public IVisitorRepo VisitorRepo => _VisitorRepo.Value;
        public IDepartmentRepo DepartmentRepo => _DepartmentRepo.Value;

		public IVisitorBlackListRepo BlackListRepo => _BlackListRepo.Value;
		public IStorageService StorageService => _storageService.Value;
		public async Task SaveAsync() => await context.SaveChangesAsync();
    }
	public class FileStorageService : IStorageService
	{
		private readonly IHostingEnvironment _env;
		private readonly string _baseUploadPath;

		public FileStorageService(IHostingEnvironment env)
		{
			_env = env;
			_baseUploadPath = Path.Combine(_env.WebRootPath, "uploads");
		}

		public async Task<string> SaveVisitorNidAsync(IFormFile file, string departmentName, DateTime visitDate)
		{
			// Format folder names
			string safeDepartment = "D" + Sanitize(departmentName) + "D";
			string visitDateFolder = "VD" + visitDate.ToString("yyyy-MM-dd") + "VD";

			// Create physical path: wwwroot/uploads/D<Department>D/VD<yyyy-MM-dd>VD/
			string departmentFolderPath = Path.Combine(_baseUploadPath, safeDepartment, visitDateFolder);

			// Create folders if they don't exist
			if (!Directory.Exists(departmentFolderPath))
				Directory.CreateDirectory(departmentFolderPath);

			// Generate file name and full path
			string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
			string fullPath = Path.Combine(departmentFolderPath, fileName);

			using (var stream = new FileStream(fullPath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			// Return relative web-accessible path (used in browser)
			string relativePath = $"/uploads/{safeDepartment}/{visitDateFolder}/{fileName}";
			return relativePath;
		}

		private string Sanitize(string input)
		{
			foreach (var c in Path.GetInvalidFileNameChars())
				input = input.Replace(c, '_');

			input = input.Replace(" ", "_"); // Optional: make it more URL-friendly
			return input;
		}
	}

}
