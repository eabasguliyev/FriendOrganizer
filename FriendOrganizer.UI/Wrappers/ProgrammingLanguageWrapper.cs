using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Wrappers
{
    public class ProgrammingLanguageWrapper:ModelWrapper<ProgrammingLanguage>
    {
        public ProgrammingLanguageWrapper(ProgrammingLanguage model) : base(model)
        {
        }

        public int Id => GetValue<int>();

        public string Name
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
    }
}