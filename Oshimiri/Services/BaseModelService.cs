using Oshimiri.Data;
using Oshimiri.Models;

namespace Oshimiri.Services
{
    public abstract class BaseModelService<T> where T : Audit
    {
        protected T UpdateAudit(T model, bool isUpdating = false)
        {
            var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);
            if (!isUpdating)
            {
                model.CreatedDate = currentDate;
            }
            model.UpdatedDate = currentDate;
            return model;
        }
    }
}
