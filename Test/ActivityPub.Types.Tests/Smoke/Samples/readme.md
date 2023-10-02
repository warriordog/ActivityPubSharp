# Real-world Samples

These samples are included to "smoke out" potential crashes or other high-level bugs in the code base.
By running real-world JSON through the library, we can ensure that interaction with that software is at least
*possible* without crashing.

**Important note:**
[the project file](../../ActivityPub.Types.Tests.csproj) includes a directive to include all ".jsonld" files in this directory / subdirectories.
To add new samples, you can simply add the files in the correct place with the appropriate extension.
There is no need to manually include the file.
If the `csproj` file must ever be regenerated, then make sure to include these lines:

```xml
    <ItemGroup>
      <None Remove="Smoke\Samples\**\*.jsonld" />
      <EmbeddedResource Include="Smoke\Samples\**\*.jsonld" />
    </ItemGroup>
```

## Current samples:

| Software                           | Type          | Notes            | Tests                                                        |
|------------------------------------|---------------|------------------|--------------------------------------------------------------|
| [Firefish (AKA Calckey)](Firefish) | Microblogging | Fork of Misskey  | [FirefishSampleTests.cs](Firefish/FirefishSampleTests.cs)    |
| [Glitch-Soc](GlitchSoc)            | Microblogging | Fork of Mastodon | [GlitchSocSampleTests.cs](GlitchSoc/GlitchSocSampleTests.cs) |

## Tips:

* Download `Person` from Mastodon / Glitch-soc / Misskey / Foundkey / Firefish (Calckey):
  ```powershell
  $output = "Person.jsonld"
  $instance = "https://example.com"
  $user = "name / id"
  Invoke-WebRequest -Headers @{"accept"="application/activity+json"} "$instance/users/$user" -OutFile $output -UseBasicParsing
  ```