# SpellChecker

Console application for checking spelling mistakes using a custom dictionary.

The application finds unknown words and suggests possible corrections based on edit distance.

## Requirements

- .NET 10 SDK

## Build

```bash
dotnet build
```

## Run

Run the application with two arguments:

```bash
dotnet run --project SpellChecker -- input.txt output.txt
```

Where:

- `input.txt` - input file with dictionary and text to check
- `output.txt` - output file with corrected text

## Input format

The input file contains two sections separated by `===`.

The first section contains dictionary words.

The second section contains text that should be checked.

Example:

```
rain spain plain plaint pain main mainly
the in on fall falls his was
===
hte rame in pain fells
mainy oon teh lain
was hints pliant
===
```

## Output rules

If a word exists in the dictionary, it remains unchanged.

Example:

```
pain
```

Output:

```
pain
```

If a single correction is found, it is applied automatically.

Example:

```
fells
```

Output:

```
falls
```

If multiple corrections are possible, all candidates are displayed:

Example:

```
mainy
```

Output:

```
{main mainly}
```

If no suitable correction is found, the word is marked as unknown:

Example:

```
hints
```

Output:

```
{hints?}
```

## Testing

Run tests:

```bash
dotnet test
```

The tests cover:

- dictionary processing
- word correction
- unknown words handling
- multiple suggestions
- case preservation
- spaces and tabs preservation
- long word processing

## Example

Input:

```
rain spain plain plaint pain main mainly
the in on fall falls his was
===
hte rame in pain fells
mainy oon teh lain
was hints pliant
===
```

Output:

```
the {rame?} in pain falls
{main mainly} on the plain
was {hints?} plaint
```