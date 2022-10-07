# Learn C# - FluentValidation

## Table of contents

* [Install](#install)
* [Configure](#configure)
* [Scenario](#scenario)

## Install
``` powershell
Install-Package FluentValidation -Version 11.2.2
```

## Configure
``` csharp
```

## Scenario
* For a given person, on a schedule slot, create an appointment
* Schedule types
  * Baby
    * Schedule slot interval between 10-30 min
    * Age between 0-1 years
  * Toddler
    * Schedule slot interval between 10-45 min
    * Age between 1-3 years
  * Child
    * Schedule slot interval between 30-60 min
    * Age between 3-12 years
  * Teen
    * Schedule slot interval between 30-90 min
    * Age between 12-18 years
  * Adult
    * Schedule slot interval between 60-120 min
    * Age greater than 18
* Ranges should be inclussive-exclussive `[)`
