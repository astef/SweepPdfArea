# SweepPdfArea

CLI tool to remove content under specified PDF document rectangle area

# Usage

```
PS> dotnet build .
...

PS> ./bin/Debug/net48/SweepPdfArea.exe `
    --input ./input.pdf`
    --output ./output.pdf`
    --fill-color 00AAFF`
    --page 1`
    -x 30`
    -y 30.5`
    -w 300`
    -h 633.05

```

Should be used before https://github.com/astef/PatchPdfText to ensure old content is really moved from the document.

# Development state

This is a not a complete product, but rather a home-made script helper intended for a particular limited case.

If you feel that it can be slightly updated to be helpful in your case as well, then go update it, or submit an issue.

# License

This tool is using itext7 under the hood, so check-out their license: https://github.com/itext/itext7-dotnet/blob/develop/LICENSE.md

In short, you can not distribute the program without commercial license, but you can use it and distribute the PDF files.
