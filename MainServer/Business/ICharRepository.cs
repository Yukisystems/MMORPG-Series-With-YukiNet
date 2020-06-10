namespace MainServer.Business
{
    public interface ICharRepository
    {
        bool CharExists(string charname);
        void SaveCharacterToDb(int UserId, string charname, string UmaData);
        void UpdateUser(int UserID, string charname);
    }
}