# System integration: fundamentals

## Table of contents
- [Project desciption](#project-description)
- [Project Setup](#project-setup)
    - [Create MMigration](#create-migration)
    - [Update database](#update-database)
    - [Create environnement variables](#create-environnment-variables)
    - [Run](#run)
- [First Launch](#u-stylecolorgreen-first-launch--u)
- [Important Information](#u-stylecolorredimportant-informations--u)
    - [To log into admin](#uto-log-into-the-admin-account-u)
    - [List of clients](#ulist-of-default-clients-u)
- [To do](#todo)

___

## Project description 

> #### This  project is a Bank application
> There are two types of users: administrators and clients.
> Administrators can manage client accounts ( creation , modification , deletion ... )
>
> Clients can retrieve, add, exchange (with another client or between two of his currencies) money.
>
> To connect, administrators have a predefined username and password and clients have a guid and a pin code.
> (see [Important information](#important-informations-))
> 
> To know more informations you can read the [subject](https://github.com/MartinLenaerts/school-ISF-project/blob/master/subject.pdf) of this project 


## Project setup

### Create Migration

```
dotnet ef migrations add [name_of_migration]
```

### Update database

```
dotnet ef database update             
```

### Create environnment variables

>create an ``` .env.local ``` with  [``` .env ```](https://github.com/MartinLenaerts/school-ISF-project/blob/master/.env) in example
>
>And set this row ``` API_KEY=xxx ```
> 
> If you haven't api key you can create an account here : [https://app.exchangerate-api.com/sign-up](https://app.exchangerate-api.com/sign-up)


### Run

```
dotnet run
```

___

## First launch !

>If it's the first start of program you can execute the seed to insert data in database.
>
>so you can tap ``` y ``` to this question :
>
>``` Do you want to insert new data in database ? Warning ! This action will remove all old data (y,N) ```
>
>by default the seed is not executed

## Important Informations !

### To log into the admin account :

>username : ``` admin ```
>
>password : ``` admin ```


___

### List of default clients :

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

## TO DO

> - [x] Unblock client
> - [x] Handle error when client have not currency
> - [ ] Test add and retrieve money
> - [ ] Add message to admin
> - [ ] Change Pin