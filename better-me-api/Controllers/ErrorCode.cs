
namespace BetterMeApi.Controllers
{
    /**
     * ENUM für Error-Handling
     */
    public enum ErrorCode
    {
        DataProvidedIsInvalid,
        ItemAlreadyExists,
        ItemNotFound,
        CouldNotCreateItem,
        CouldNotUpdateItem,
        CouldNotDeleteItem,
    }
}