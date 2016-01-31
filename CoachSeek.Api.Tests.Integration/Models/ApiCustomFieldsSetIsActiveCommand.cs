namespace CoachSeek.Api.Tests.Integration.Models
{
    public class ApiCustomFieldsSetIsActiveCommand
    {
        public string commandName { get { return "CustomFieldTemplateSetIsActive"; } }
        public bool isActive { get; set; }
    }
}
