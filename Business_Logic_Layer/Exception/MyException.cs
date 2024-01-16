namespace StudentProjectMVC.Exception
{
    public class MyException:ApplicationException
    {
        public string AgeException()
        {
            string message = "Please Enter Valid Age";
            return message;
        }
        public string EmailException()
        {
            string message = "Please Enter Valid Email";
            return message;
        }
    }
}
