using System.Threading;

namespace CreditCards.UITests
{
    internal static class DemoHelper
    {
        public static void Pause(int mSec = 2000)
        {
            Thread.Sleep(mSec);
        }
    }
}
