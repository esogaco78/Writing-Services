#pragma checksum "D:\ASPNET\Tycoon\Tycoon\Areas\Admin\Views\Service\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d2c05b0e7279eb34cffd3f340ccfbfb9c4ede14e"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Admin_Views_Service_Index), @"mvc.1.0.view", @"/Areas/Admin/Views/Service/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\ASPNET\Tycoon\Tycoon\Areas\Admin\Views\_ViewImports.cshtml"
using Tycoon;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\ASPNET\Tycoon\Tycoon\Areas\Admin\Views\_ViewImports.cshtml"
using Tycoon.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d2c05b0e7279eb34cffd3f340ccfbfb9c4ede14e", @"/Areas/Admin/Views/Service/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"239899a5d1ed9d4445c0d6c1c2b12f17aa0552b0", @"/Areas/Admin/Views/_ViewImports.cshtml")]
    public class Areas_Admin_Views_Service_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<Service>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "_CreateButtonPartial", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "_TableButtonsPartial", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "D:\ASPNET\Tycoon\Tycoon\Areas\Admin\Views\Service\Index.cshtml"
  
    ViewData["Title"] = "Index";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"border backgroundwhite\">\r\n    <div class=\"row\">\r\n        <div class=\"col-6\">\r\n            <h2 class=\"text-info\">Service List</h2>\r\n        </div>\r\n        <div class=\"col-6 text-right\">\r\n            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("partial", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "d2c05b0e7279eb34cffd3f340ccfbfb9c4ede14e4168", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Name = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n        </div>\r\n    </div>\r\n    <br />\r\n    <div>\r\n");
#nullable restore
#line 18 "D:\ASPNET\Tycoon\Tycoon\Areas\Admin\Views\Service\Index.cshtml"
         if (Model.Count() > 0)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <table class=\"table table-striped border\">\r\n                <tr class=\"table-secondary\">\r\n                    <th>\r\n                        ");
#nullable restore
#line 23 "D:\ASPNET\Tycoon\Tycoon\Areas\Admin\Views\Service\Index.cshtml"
                   Write(Html.DisplayNameFor(m => m.Name));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </th>\r\n                    <th>\r\n                        ");
#nullable restore
#line 26 "D:\ASPNET\Tycoon\Tycoon\Areas\Admin\Views\Service\Index.cshtml"
                   Write(Html.DisplayNameFor(m => m.Price));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </th>\r\n                    <th>\r\n                        ");
#nullable restore
#line 29 "D:\ASPNET\Tycoon\Tycoon\Areas\Admin\Views\Service\Index.cshtml"
                   Write(Html.DisplayNameFor(m => m.CategoryId));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </th>\r\n                    <th>\r\n                        ");
#nullable restore
#line 32 "D:\ASPNET\Tycoon\Tycoon\Areas\Admin\Views\Service\Index.cshtml"
                   Write(Html.DisplayNameFor(m => m.SubCategoryId));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </th>\r\n                    <th>\r\n\r\n                    </th>\r\n                    <th>\r\n\r\n                    </th>\r\n                </tr>\r\n");
#nullable restore
#line 41 "D:\ASPNET\Tycoon\Tycoon\Areas\Admin\Views\Service\Index.cshtml"
                 foreach (var service in Model)
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <tr>\r\n                <td>\r\n                    ");
#nullable restore
#line 45 "D:\ASPNET\Tycoon\Tycoon\Areas\Admin\Views\Service\Index.cshtml"
               Write(Html.DisplayFor(m => service.Name));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n                <td>\r\n                    ");
#nullable restore
#line 48 "D:\ASPNET\Tycoon\Tycoon\Areas\Admin\Views\Service\Index.cshtml"
               Write(Html.DisplayFor(m => service.Price));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n                <td>\r\n                    ");
#nullable restore
#line 51 "D:\ASPNET\Tycoon\Tycoon\Areas\Admin\Views\Service\Index.cshtml"
               Write(Html.DisplayFor(m => service.Category.Name));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n                <td>\r\n                    ");
#nullable restore
#line 54 "D:\ASPNET\Tycoon\Tycoon\Areas\Admin\Views\Service\Index.cshtml"
               Write(Html.DisplayFor(m => service.SubCategory.Name));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n                <td>\r\n                    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("partial", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "d2c05b0e7279eb34cffd3f340ccfbfb9c4ede14e8676", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Name = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
#nullable restore
#line 57 "D:\ASPNET\Tycoon\Tycoon\Areas\Admin\Views\Service\Index.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Model = service.Id;

#line default
#line hidden
#nullable disable
            __tagHelperExecutionContext.AddTagHelperAttribute("model", __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Model, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                </td>\r\n            </tr>\r\n");
#nullable restore
#line 60 "D:\ASPNET\Tycoon\Tycoon\Areas\Admin\Views\Service\Index.cshtml"

                }

#line default
#line hidden
#nullable disable
            WriteLiteral("            </table>\r\n");
#nullable restore
#line 63 "D:\ASPNET\Tycoon\Tycoon\Areas\Admin\Views\Service\Index.cshtml"

        }
        else
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <p>There are no Services to display!!</p>\r\n");
#nullable restore
#line 68 "D:\ASPNET\Tycoon\Tycoon\Areas\Admin\Views\Service\Index.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    </div>\r\n</div>\r\n\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<Service>> Html { get; private set; }
    }
}
#pragma warning restore 1591