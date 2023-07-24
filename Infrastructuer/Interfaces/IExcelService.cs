

using System.Collections.Generic;
namespace Infrastructure.Interfaces
{
    public interface IExcelService
    {
        string GenerateExcel<T>(IEnumerable<T> records, string path);
    }
}

