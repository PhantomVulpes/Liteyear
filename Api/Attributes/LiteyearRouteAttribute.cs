using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Vulpes.Liteyear.Api.Attributes;

public class LiteyearRouteAttribute : RouteAttribute
{
    public LiteyearRouteAttribute([StringSyntax("Route")] string template) : base($"api/{template}") { }
}
