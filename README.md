# InlineZipper

InlineZipper is a BOF.NET that allows operators to compress files and folders in memory, and download it without touching the disk.

It is inspired by @Outflanknl's [Zipper](https://github.com/outflanknl/Zipper) project, and uses BOF.NET's DownloadFile API.

Zip operation is performed using [DotNetZip](https://github.com/haf/DotNetZip.Semverd)

## Usage

First, you must compile [BOF.NET](https://github.com/CCob/BOF.NET) from @CCob and load the bofnet.cna.

A bofnet.dll is added to the project but you can replace it with the one you compiled.

InlineZipper uses [dnMerge](https://github.com/CCob/dnMerge) from @CCob to merge the assemblies: it must be a release build to get the assemblies merged.

Once you compiled the InlineZipper project, you can execute as follows:
```
bofnet_init
bofnet_load /path/to/InlineZipper.exe
bofnet_execute InlineZipper.Execute <path to file/folder> <path to file/folder> <...>
```

An example of a CNA that uses InlineZipper is provided, you must initialize BOF.NET and load the assembly before using it.

## Acknowledgments

[Zipper](https://github.com/outflanknl/Zipper) from @Outflanknl

[BOF.NET](https://github.com/CCob/BOF.NET) and [dnMerge](https://github.com/CCob/dnMerge) from @CCob

[DotNetZip](https://github.com/haf/DotNetZip.Semverd) from @haf