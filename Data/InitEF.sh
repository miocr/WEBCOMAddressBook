dotnet ef database drop --verbose --force --context AddressBookDbContext
dotnet ef migrations remove --verbose  --force --context AddressBookDbContext
dotnet ef migrations add Initial --verbose --context AddressBookDbContext
dotnet ef database update --verbose --context AddressBookDbContext