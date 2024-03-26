namespace GameTally.Enums
{
    enum UserStatus
    {
        NONE = 0,
        OFFLINE = 1,
        IN_GAME = 2,
    }

    public class Convertor
    {
        public static string UserStatusConvertor(int userStatus)
        {
            switch (userStatus)
            {
                case (int)UserStatus.IN_GAME:
                    return "Plays ";
                case (int)UserStatus.OFFLINE:
                    return "Offline";
            }

            return null;
        }   
    }
}