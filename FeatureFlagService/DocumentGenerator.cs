using System.Text;
using FeatureFlagService.Model;

namespace FeatureFlagService;

public class DocumentGenerator : IDocumentGenerator
{
    public string Generate(IReadOnlyCollection<FeatureFlag> featureFlags)
    {
        var docBuilder = new StringBuilder(featureFlags.Count * 200);

        docBuilder.Append(GetTableHeader());

        foreach (var flag in featureFlags)
        {
            docBuilder.Append("<tr>");
            docBuilder.Append($"<td><p>{flag.Name}</p></td>");
            docBuilder.Append(flag.ProdStatus ? GetColumnEnabled() : GetColumnDisabled());
            docBuilder.Append(flag.TestStatus ? GetColumnEnabled() : GetColumnDisabled());
            docBuilder.Append(flag.DevStatus ? GetColumnEnabled() : GetColumnDisabled());
            docBuilder.Append("</tr>");
        }
        
        docBuilder.Append("</tbody></table>");

        return docBuilder.ToString().ReplaceLineEndings(string.Empty);
    }

    private string? GetColumnDisabled()
    {
        return """
                <td><p style="text-align: center;"><ac:emoticon ac:name="cross" ac:emoji-shortname=":cross_mark:" ac:emoji-id="atlassian-cross_mark" ac:emoji-fallback=":cross_mark:" /> </p></td>
               """;
    }

    private string GetColumnEnabled()
    {
        return """
               <td><p style="text-align: center;"><ac:emoticon ac:name="blue-star" ac:emoji-shortname=":white_check_mark:" ac:emoji-id="2705" ac:emoji-fallback="✅" /> </p></td>
               """;
    }

    private string GetTableHeader()
    {
        return """
               <table data-table-width="760" data-layout="default" ac:local-id="5f37e59e-3645-4028-8bff-72adf148fa50">
               <colgroup>
                   <col style="width: 405.0px;" />
                   <col style="width: 115.0px;" />
                   <col style="width: 103.0px;" />
                   <col style="width: 130.0px;" />
               </colgroup>
               <tbody>
                   <tr>
                       <th><p style="text-align: center;"><strong>Name</strong></p></th>
                       <th><p style="text-align: center;"><strong>Production</strong></p></th>
                       <th><p style="text-align: center;"><strong>Test</strong></p></th>
                       <th><p style="text-align: center;"><strong>Development</strong></p></th>
                   </tr>
               """;
    }
}