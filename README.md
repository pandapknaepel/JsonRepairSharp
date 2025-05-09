[![.NET](https://github.com/thijse/JsonRepairSharp/actions/workflows/dotnet.yml/badge.svg)](https://github.com/thijse/JsonRepairSharp/actions/workflows/dotnet.yml)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

![JSOn Repair logo](/Assets/logo_small.png?raw=true )
# JsonRepair Sharp

## About this Fork

This fork of [JsonRepairSharp](https://github.com/thijse/JsonRepairSharp) introduces several improvements and modifications aimed at making the library more robust and accessible for .NET developers.

### Key Enhancements:

- **NuGet Package Publishing:** This fork is intended to be published as a NuGet package, making it easier to integrate JsonRepairSharp into your projects.
- **Multi-Framework Support:** The project now supports both .NET 8 and .NET 9, ensuring maximum compatibility with current and future .NET projects while leveraging the latest framework features and performance improvements.
- **Thread-Safety Improvements:** The `JsonRepair` class has been refactored to remove static properties. The `RepairJson` method now takes parameters instead of relying on static configuration, allowing for safe concurrent execution.

By maintaining the original functionality of JsonRepairSharp while making these refinements, this fork enhances usability, performance, and maintainability.

## Installation

The updated **JsonRepairSharp** library is available as a NuGet package.

**NuGet Package:**  
🔗 [jsonrepairsharp on NuGet](https://www.nuget.org/packages/jsonrepairsharp/)

### Install via .NET CLI:
```sh
dotnet add package jsonrepairsharp
```

### Install via NuGet Package Manager:
```sh
Install-Package jsonrepairsharp
```

This allows you to quickly integrate the library into your .NET project and use its JSON repair functionality efficiently.

## About this project

Jsonrepair Sharp is a near-literal translation of the TypeScript JsonRepair library, see https://github.com/josdejong/jsonrepair

The jsonrepair library is basically an extended JSON parser. It parses the provided JSON document character by character. When it encounters a non-valid JSON structures it wil look to see it it can reconstruct the intended JSON. For example, after encountering an opening bracket {, it expects a key wrapped in double quotes. When it encounters a key without quotes, or wrapped in other quote characters, it will change these to double quotes instead.

The library has many uses, such as:

1. Convert from an a Word document
1. Convert from objects with a JSON-like structure, such as Javascript
1. Convert from a string containing a JSON document
1. Convert from MongoDB output
1. Convert from Newline Delimited JSON logs
1. Convert from JSON dialects
1. Convert from Truncated or corrupted JSON.

But with the advent of Language Model Models (LLMs) there is yet another use-case. LLMs are notoriously bad in consistently outputting well-formed datastructures, even as simple as JSON. Requiry-ing is expensive and time consuming.  Jsonrepair comes to the rescue by repair these JSON modules files, and increasing changes of smooth processing.

*The library can fix the  following issues:*

- Add missing quotes around keys
- Add missing escape characters
- Add missing commas
- Add missing closing brackets
- Replace single quotes with double quotes
- Replace special quote characters like `“...”`  with regular double quotes
- Replace special white space characters with regular spaces
- Replace Python constants `None`, `True`, and `False` with `null`, `true`, and `false`
- Strip trailing commas
- Strip comments like `/* ... */` and `// ...`
- Strip JSONP notation like `callback({ ... })`
- Strip escape characters from an escaped string like `{\"stringified\": \"content\"}`
- Strip MongoDB data types like `NumberLong(2)` and `ISODate("2012-12-19T06:01:17.171Z")`
- Concatenate strings like `"long text" + "more text on next line"`
- Turn newline delimited JSON into a valid JSON array, for example:
    ```
    { "id": 1, "name": "John" }
    { "id": 2, "name": "Sarah" }
    ```

*In LLM mode*
- Strip characters heading the JSON opening brace and  trailing the JSON closing brace.


## Use

Use the original typescript version in a full-fledged application: https://jsoneditoronline.org
Read the background article ["How to fix JSON and validate it with ease"](https://jsoneditoronline.org/indepth/parse/fix-json/)

Here's the updated code example that reflects the changes made in your fork:

---

## Code Example (Updated in Fork)

In this fork, the `RepairJson` method has been updated to remove static properties, making it thread-safe. The method now accepts parameters instead of relying on global settings.

### **Before (Original Code)**
```cs
// Enable throwing exceptions when JSON code can not be repaired or even understood (enabled by default)
JsonRepair.ThrowExceptions = true;

// Set context as LLM or Other. This will repair the json slightly differently. (Other by default)
JsonRepair.Context = Other;

try
{
    string json = "{name: 'John'}";
    string repaired = JsonRepair.JsonRepair(json);
    Console.WriteLine(repaired);
    // Output: {"name": "John"}
}
catch (JSONRepairError err)
{
    Console.WriteLine(err.Message);
    Console.WriteLine("Position: " + err.Data["Position"]);
}
```

### **Now (Updated in Fork)**
```cs
try
{
    // The following is invalid JSON: it consists of JSON contents copied from 
    // a JavaScript code base, where the keys are missing double quotes, 
    // and strings are using single quotes:
    string json = "{name: 'John'}";

    // New method signature using parameters instead of static properties
    string repaired = JsonRepair.RepairJson(json, InputType.Other, true);
    
    Console.WriteLine(repaired);
    // Output: {"name": "John"}
}
catch (JSONRepairError err)
{
    Console.WriteLine(err.Message);
    Console.WriteLine("Position: " + err.Data["Position"]);
}
```

### **What Changed?**
- `RepairJson` now takes parameters (`inputType` and `throwsException`) instead of relying on static properties.
- The class is now thread-safe because no shared state (`ThrowExceptions`, `Context`) is used.
- The method call is explicit and does not require setting global configuration.

This change improves maintainability and makes the library safe for concurrent execution in multi-threaded environments.

### Command Line Interface (CLI)

The github archive comes with a `jsonrepair` cli tool, it can be used on the command line. To use, build JsonRepair-CLI.

Usage:

```
$ jsonrepair "inputfilename.json" {OPTIONS}
```

Options:

```
--version,   -v                       Show application version
--help,      -h                       Show help
--new,       -n "outputfilename.json" Write to new file
--overwrite, -o                       Replace the input file
--llm,       -l                       Parse in LLM mode
```

Example usage:

```
$ jsonrepair "broken.json"                         # Repair a file, output to console
$ jsonrepair "broken.json" > "repaired.json"       # Repair a file, output to command line and pipe to file
$ jsonrepair "broken.json" -n -l "repaired.json"   # Repair a file in LLM mode, output to command line and pipe to file
$ jsonrepair "broken.json" --overwrite             # Repair a file, replace the input json file
```

### GUI

The archive also comes with a minimal GUI that shows a somewhat simplistic diff between the original and fixed JSON
![JSOn Repair GUI](/Assets/JsonRepairGui.png?raw=true )
This GUI heavily leans on the awesome [FastColoredTextBox](https://github.com/PavelTorgashov/FastColoredTextBox) library and the the diff sample in particular.



## Alternatives:

Similar libraries:

- https://github.com/josdejong/jsonrepair
- https://github.com/RyanMarcus/dirty-json

## Acknowledgements

Thanks go out to Jos de Jong, who not only did all the heavy lifting in the original jsonrepair for typescript library, but also patiently helped getting this port to pass all unit tests. 

## License

Released under the [MITS license](LICENSE.md).
