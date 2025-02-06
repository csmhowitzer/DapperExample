# A .NET 9 Dapper + Minimal API Example Project

Based on the **example.db** Sqlite repo, a example project was made. Using Dapper and minimal apis to showcase / for-reference how-to set up and configure such a project. 
Not all items are mapped, but there are single items, one-to-one mappings, and one-to-many mappings.

At the time, minimal API's were highly optimized for speed in the .NET's most recent updates (9.0.1 Preview). The solution builds in ~1.8s on average and each request (single transactions) is fast as well.

DB currently totals around 500 records after initial seeding.
