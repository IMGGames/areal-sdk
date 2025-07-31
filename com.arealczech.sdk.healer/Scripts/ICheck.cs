using System.Collections.Generic;

namespace Areal.SDK.Healer {
    internal interface ICheck {
        IEnumerable<ICheckResult> Check();

        void Fix(ICheckResult checkResult);
    }
}
