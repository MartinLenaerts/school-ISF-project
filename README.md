# System integration: fundamentals

## Project setup

### Create Migration

```
dotnet ef migrations add [name_of_migration]
```

### Update database Migration

```
dotnet ef database update             
```

### Run

```
dotnet run
```

## <u style="color:green"> First launch ! </u>

If it's the first start of program you can execute the seed to insert data in database.

so you can tap ``` y ``` to this question :

``` Do you want to insert new data in database ? Warning ! This action will remove all old data (y,N) ```

by default the seed is not executed

## <u style='color:red'>Important Informations ! </u>

### <u>To log into the admin account :</u>

username : ``` admin ```

password : ``` admin ```

### <u>List of default clients :</u>

```
----------------------------------------------------------------------------------------------
|  CLIENTS  NAMES : Nicolas Sarkosy , Francois Hollande , Jacques Chirac , Emmanuel Macron   |
|                                                                                            |
|  DETAILS :                                                                                 |
|                                                                                            |
|  Client n°1                                                                                |
|         Firstname : Nicolas                                                                |
|         LastName : Sarkosy                                                                 |
|         isBlocked : False                                                                  |
|         Tries : 0                                                                          |
|         Pin : 1234                                                                         |
|         Currencies :                                                                       |
|                     - 12000 EUR                                                            |
|                     - 7867812 USD                                                          |
|  Client n°2                                                                                |
|         Firstname : Francois                                                               |
|         LastName : Hollande                                                                |
|         isBlocked : False                                                                  |
|         Tries : 0                                                                          |
|         Pin : 1234                                                                         |
|         Currencies :                                                                       |
|                     - 2453 EUR                                                             |
|                     - 7864 USD                                                             |
|  Client n°3                                                                                |
|         Firstname : Jacques                                                                |
|         LastName : Chirac                                                                  |
|         isBlocked : False                                                                  |
|         Tries : 0                                                                          |
|         Pin : 1234                                                                         |
|         Currencies :                                                                       |
|                     - 453 EUR                                                              |
|                     - 21137 USD                                                            |
|  Client n°4                                                                                |
|         Firstname : Emmanuel                                                               |
|         LastName : Macron                                                                  |
|         isBlocked : False                                                                  |
|         Tries : 0                                                                          |
|         Pin : 1234                                                                         |
|         Currencies :                                                                       |
|                     - 453 EUR                                                              |
|                     - 78624 USD                                                            |
|                                                                                            |
----------------------------------------------------------------------------------------------
```