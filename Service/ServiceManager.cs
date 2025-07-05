using AutoMapper;
using Core.Contracts;
using Core.Entities;
using Core.RepositoryContracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Repository;
using Service.Service;
using Service.Services;


namespace Service
{

    public class ServiceManager:IServiceManager
    {

        private Lazy<IUserService> _UserService;
        private Lazy<ICardService> _CardService;
        private Lazy<IFloorService> _FloorService;
        private Lazy<IVisitorService> _VisitorService;
        private Lazy<IVisitService> _VisitService;
        private Lazy<IDepartmentService> _DepartmentService;
        private Lazy<IVisitorBlackListService> _VisitorBlackListService;
		

		public ServiceManager(IRepositoryManager repositoryManager,IMapper mapper,ILoggerManager logger,UserManager<User> userManager,IConfiguration configuration,RoleManager<IdentityRole> roleManager)
        { 
            _UserService = new Lazy<IUserService>(() => new UserService(repositoryManager, mapper,logger,userManager,configuration,roleManager));
            _CardService = new Lazy<ICardService>(() => new CardService(repositoryManager, mapper, logger));
            _FloorService = new Lazy<IFloorService>(() => new FloorService(repositoryManager, mapper,logger));
            _VisitorService = new Lazy<IVisitorService>(() => new VisitorService(repositoryManager, mapper,logger));
            _VisitService = new Lazy<IVisitService>(() => new VisitService(repositoryManager, mapper,logger));
            _DepartmentService = new Lazy<IDepartmentService>(() => new DepartmentService(repositoryManager, mapper, logger));
			_VisitorBlackListService = new Lazy<IVisitorBlackListService>(() => new VisitorBlackListService(repositoryManager, mapper, logger));
			
		}

        public IFloorService FloorService => _FloorService.Value;
        public ICardService CardService =>_CardService.Value;
        public IVisitorService VisitorService => _VisitorService.Value;
        public IVisitService VisitService =>_VisitService.Value;
        public IUserService UserService => _UserService.Value;
        public IDepartmentService DepartmentService => _DepartmentService.Value;
		public IVisitorBlackListService VisitorBlackListService => _VisitorBlackListService.Value;
		
	}
}
