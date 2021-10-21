namespace Bank.Models
{
    public class Admin
    {
        private string password;
        private string username;
        private static ClientDBAccess ClientDbAccess = new ClientDBAccess();
        private static ClientJsonAccess ClientJsonAccess = new ClientJsonAccess();
        public Admin(string password, string username)
        {
            this.password = password;
            this.username = username;
        }

        public string Password
        {
            get => password;
            set => password = value;
        }

        public string Username
        {
            get => username;
            set => username = value;
        }

    }
}