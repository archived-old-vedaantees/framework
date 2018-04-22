dotnet pack Vedaantees.Framework -o D:\framework
dotnet pack Vedaantees.Framework.Providers -o D:\framework
nuget push *.nupkg -Source "https://www.nuget.org/api/v2/package" -ApiKey oy2nbipr5yctq72t64tq6ou3m2fwl5fuxiaaovbys3r2l4
del *.nupkg