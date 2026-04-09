namespace LMS.Core.Resources
{
    public static class SharedResourcesKeys
    {



        #region All
        public const string Required = "Required";
        public const string Error = "Error";
        public const string NotFound = "NotFound";
        public const string Deleted = "Deleted";
        public const string Retrieved = "Retrieved";
        public const string UnAuthorized = "UnAuthorized";
        public const string Updated = "Updated";
        public const string Created = "Created";
        public const string InvalidId = "InvalidId";
        public const string NotEmpty = "NotEmpty";
        public const string MaximumLengthIs100 = "MaximumLengthIs100";
        public const string MaximumLengthIs2000 = "MaximumLengthIs2000";
        public const string IsNullOrWhiteSpace = "IsNullOrWhiteSpace";
        public const string FailedDeleted = "FailedDeleted";
        public const string FailedAdded = "FailedAdded";
        public const string FailedUpdated = "FailedUpdated";
        public const string GreaterThan0 = "GreaterThan0";
        public const string NameMustBeBetween3And100 = "NameMustBeBetween3And100";

        #endregion

        #region Role 
        public const string RoleNameMustBeUnique = "RoleNameMustBeUnique";
        public const string Cannotupdaterolename = "Cannotupdaterolename";
        public const string CannotDeleterolename = "CannotDeleterolename";
        #endregion

        #region Category
        public const string CategoryAdded = "CategoryAdded";
        public const string CategoryUpdated = "CategoryUpdated";
        public const string CategoryDeleted = "CategoryDeleted";
        public const string CategoryRetrieved = "CategoryRetrieved";
        public const string CategoriesRetrieved = "CategoriesRetrieved";
        public const string CategoryNotFound = "CategoryNotFound";

        public const string CategoryNameMustBeUnique = "CategoryNameMustBeUnique";

        #endregion

        #region User
        public const string UserNotFound = "UserNotFound";
        public const string PasswordIsWrong = "Password";
        public const string EmailNotFound = "EmailNotFound";
        public const string Confirmpassworddoesnotmatch = "Confirmpassworddoesnotmatch";
        public const string UserLoggedOutSuccessfully = "UserLoggedOutSuccessfully";
        public const string SendEmailFailed = "SendEmailFailed";
        public const string NotConfirmEmail = "NotConfirmEmail";
        public const string ConfirmEmail = "ConfirmEmail";
        public const string EmailSendFailed = "EmailSendFailed";
        public const string validemailaddress = "validemailaddress";
        public const string UserWithEmailIsAlreadyFound = "UserWithEmailIsAlreadyFound";
        public const string EmailMustBeUniqueItIsFoundAlready = "EmailMustBeUniqueItIsFoundAlready";
        public const string PleaseLogIn = "PleaseLogIn";

        #endregion

        #region InstructorProfile

        public const string Requestnotfound = "Requestnotfound";
        public const string LinkedInURL = "LinkedInURL";
        public const string RateBetween0And5 = "RateBetween0And5";
        public const string InstructorprofileRequestalreadyexists = "InstructorprofileRequestalreadyexists";
        public const string Instructorprofileisnotapproved = "Instructorprofileisnotapproved";
        public const string YouDonthaveanyrequest = "YouDonthaveanyrequest";
        #endregion

        #region Courses And Lessons

        //Courses

        public const string NocoursesfoundforThiscategory = "NocoursesfoundforThiscategory";
        public const string NocoursesfoundforThisInstructor = "NocoursesfoundforThisInstructor";
        public const string Imagesize = "Imagesize";
        public const string ImageType = "Imagetype";
        public const string Invalidcourselevel = "Invalidcourselevel";
        public const string Invalidcourselanguage = "Invalidcourselanguage";
        public const string CourseDonotHaveAnyLessons = "CourseDonotHaveAnyLessons";
        public const string Durationmustnotexceed10hours = "Durationmustnotexceed10hours";
        public const string Coursenotapproved = "Coursenotapproved";
        public const string ordernumberisalreadyused = "ordernumberisalreadyused";
        public const string CourseNotFound = "CourseNotFound";
        public const string NoCoursesFound = "NoCoursesFound";


        public const string UserNotEnrolledInThisCourse = "UserNotEnrolledInThisCourse";

        //Lessons

        public const string LessonNotFound = "LessonNotFound";
        public const string LessonAdded = "LessonAdded";
        public const string LessonUpdated = "LessonUpdated";
        public const string LessonDeleted = "LessonDeleted";
        public const string LessonRetrieved = "LessonRetrieved";
        public const string LessonsRetrieved = "LessonsRetrieved";






        #endregion



        #region Subscription And Plans
        public const string Onlypendingrequestscanbecancelled = "Onlypendingrequestscanbecancelled";
        public const string Cannotchangeplanforanexpiredsubscription = "Cannotchangeplanforanexpiredsubscription";
        public const string LessThanOrEqualTo24Months = "LessThanOrEqualTo24Months";
        public const string SubscriptionIsAlreadyFound = "SubscriptionIsAlreadyFound";
        public const string SubscriptionNotFound = "SubscriptionNotFound";
        public const string Subscriptionisexpired = "Subscriptionisexpired";
        public const string SubscriptionisalreadyActive = "Subscriptionisexpired";

        public const string InvalidDiscountType = "InvalidDiscountType";
        public const string YouCannotDeleteAccountYouSubscribeInCourse = "YouCannotDeleteAccountYouSubscribeInCourse";

        ///plan
        public const string PlanNotFound = "PlanNotFound";
        public const string PlanAdded = "PlanAdded";
        public const string PlanUpdated = "PlanUpdated";
        public const string PlanDeleted = "PlanDeleted";
        public const string PlanRetrieved = "PlanRetrieved";
        public const string PlansRetrieved = "PlansRetrieved";







        #endregion

        #region Code
        public const string CodeSent = "CodeSent";
        public const string CodeIsWrong = "CodeIsWrong";
        public const string InvalidCouponCode = "InvalidCouponCode";
        public const string YouHaveAlreadyUsedThisCoupon = "YouHaveAlreadyUsedThisCoupon";


        #endregion












    }
}
