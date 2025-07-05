using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    public abstract class NotFoundException : Exception
    {
        protected NotFoundException(string? message) : base(message)
        {
        }
    }

    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(string id) :
            base($"لا يوجد مستخدم بهذا الاسم")
        //base($"no User exists with this id {id}")
        {
        }
    }
    
    public class VisitNotFoundException : NotFoundException
    {
        public VisitNotFoundException(long visitId) : base($"لا توجد زياره مسجله بهذا المعرف {visitId}")
        {
        }
    }
    public class VisitorNotFoundException : NotFoundException
	{
		public VisitorNotFoundException() : base($"لا يوجد زائر بهذا المعرف ")
		{
		}
		public VisitorNotFoundException(string NID) : base($" لا يوجد زائر بهذا الرقم القومي او الباسبور {NID}")
		{
		}
	}
    public class VisitorBlackListNotFoundException : NotFoundException
	{
		public VisitorBlackListNotFoundException() : base($"لا يوجد زائر محظور بهذا المعرف ")
		{
		}
	}


	public class DepartmentNotFoundException : NotFoundException
    {
        public DepartmentNotFoundException(string Id) : base($"لا توجد اداره مسجله بهذا المعرف {Id}")
        {
        }
    }
	
	public class FloorNotFoundException : NotFoundException
	{
		public FloorNotFoundException() : base($"لا يوجد طابق بهذا المعرف ")
		{
		}
	}
    public class CardNotFoundException : NotFoundException
	{
		public CardNotFoundException() : base($"لا يوجد كارت بهذا الرقم")
		{
		}
	}

}
