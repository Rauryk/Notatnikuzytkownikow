using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using System.Diagnostics;

namespace Notatnikuzytkownikow.Models
{
    public class DynamicTable
    {
        public List<Dictionary<string, string>> properties { get; set; }

    }

}
