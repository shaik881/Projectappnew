namespace DefaultIdentityColumnRename.ViewModels
{
    public class ProfileViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email {  get; set; }
        public DateTime DOB { get; set; }
        public string MobileNumber {  get; set; }
        public string PinCode {  get; set; }
        public string State {  get; set; }
        public string Country {  get; set; }
        public string Address {  get; set; }
        public string Gender { get; set; }
        public IFormFile Pic { get; set; }
        public byte[] ShowPic { get; set; }
    }
}
