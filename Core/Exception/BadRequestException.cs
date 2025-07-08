
namespace Core.Exceptions
{
    public abstract class BadRequestException:Exception
    {
         public BadRequestException(string messsage):base(messsage)
        {
            
        }
        public sealed class RefreshTokenBadRequest : BadRequestException
        {
            public RefreshTokenBadRequest()
            : base("برجاء الخروج والدخول مرة اخري ")
            //: base("Invalid client request. The tokenDto has some invalid values.")
            {
            }
        }
        public class DepartmentBadRequestException : BadRequestException
        {
            public DepartmentBadRequestException(string deptName) : base($"هذه الاداره {deptName}موجوده بالفعل")
            {
            }
        }
        public class CardAlreadyExistsBadRequestException : BadRequestException
		{
			public CardAlreadyExistsBadRequestException(string cardNumber) : base($"الكارت {cardNumber} موجود بالفعل")
			{
			}
		}
		public class FloorAlreadyExistsBadRequestException : BadRequestException
		{
			public FloorAlreadyExistsBadRequestException(string floorName) : base($"الدور {floorName} موجود بالفعل")
			{
			}
		}


		public class DeptContainAlreadyAccountBadRequestException : BadRequestException
		{
			public DeptContainAlreadyAccountBadRequestException() : base($"هذه الاداره موجود بها حساب بالفعل برجاء التواصل مع اداره النظم لمزيد من المعلومات")
			{
			}
		}
        public class CardAlreadyAssignedException : BadRequestException
        {
			public CardAlreadyAssignedException() : base($"الكارت غير متاح حاليا")
			{
			}
		}
        public class CannotAddVisitorToAcceptedVisitBadRequestException : BadRequestException
		{
			public CannotAddVisitorToAcceptedVisitBadRequestException(string source) : base($"لا يمكن اضافه زائر لهذه  الزياره لانه تم الموافقة عليها من  قبل {source} ")
			{
			}
		}
        public class CannotAddVisitorToRejectedVisitBadRequestException : BadRequestException
        {
			public CannotAddVisitorToRejectedVisitBadRequestException(string source) : base($"لا يمكن اضافه زائر لهذه الزياره لانه تم رفضها من قبل  {source} ") { }

		}

        public class VisitorNotHaveCardToEnterBadRequest : BadRequestException
		{
			public VisitorNotHaveCardToEnterBadRequest() : base($"الزائر ليس لديه كارت للدخول")
			{
			}
		}
        public class CannotApproveVisitWithoutCardException : BadRequestException
		{
			public CannotApproveVisitWithoutCardException() : base($"لا يمكن الموافقة على الزيارة بدون كارت للزائر")
			{
			}
		}
		public class VisitorBlockedBadRequsetException : BadRequestException
		{
			public VisitorBlockedBadRequsetException(string visitorNIDorPassportNum) : base($"الزائر {visitorNIDorPassportNum} محظور من دخول المبنى")
			{
			}
		}
		
		public class CardNotFoundBadRequestException : BadRequestException
		{
			public CardNotFoundBadRequestException(long cardId) : base($"الكارت بالرقم {cardId} غير موجود")
			{
			}
		}
		 
		public class DepartmentDeleteBadRequestException : BadRequestException
		{
			public DepartmentDeleteBadRequestException(string deptId) : base($"لا يمكن حذف هذه الاداره {deptId} لانه يوجد بها حسابات")
			{
			}
		}
        public class VisitorAlreadyBlockedBadRequestException : BadRequestException
        {
            public VisitorAlreadyBlockedBadRequestException(string NID) : base($"هذا الزائر محظور بالفعل {NID}")
            {
            }
        }

        public class CannotUpdateVisitStateAfterPoliceTakeActionBadRequestException : BadRequestException
        {
            public CannotUpdateVisitStateAfterPoliceTakeActionBadRequestException() : base("لا يمكن تعديل حاله الزياره لانه تم اخذ اجراء من قبل اداره الامن")
            {
            }
        }




    }
   
}
