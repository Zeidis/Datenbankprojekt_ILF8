# Controller Usage HOWTO

This file shows how to use the CRUD controllers added to the server.

Base URL
- Assume server runs at `https://localhost:5001` (adjust port as configured in launchSettings).

Endpoints (summary)
- **Launch**: GET `/api/Launch`, GET `/api/Launch/{id}`, POST `/api/Launch`, PUT `/api/Launch/{id}`, DELETE `/api/Launch/{id}`
- **Location**: GET `/api/Location`, GET `/api/Location/{id}`, POST `/api/Location`, PUT `/api/Location/{id}`, DELETE `/api/Location/{id}`
- **MoonPhaseEntity**: GET `/api/MoonPhaseEntity`, GET `/api/MoonPhaseEntity/{id}`, POST `/api/MoonPhaseEntity`, PUT `/api/MoonPhaseEntity/{id}`, DELETE `/api/MoonPhaseEntity/{id}`
- **AnalysisCache**: GET `/api/AnalysisCache`, GET `/api/AnalysisCache/{id}`, POST `/api/AnalysisCache`, PUT `/api/AnalysisCache/{id}`, DELETE `/api/AnalysisCache/{id}`

Notes about models
- `Launch` primary key is `LaunchId` (string). Provide a unique value when creating, or change controller to generate GUIDs.
- `Location` primary key is `LocationId` (int). To create a nested `Location` with a `Launch`, set `locationId` to `0` or omit it.
- Controllers set timestamps (`CreatedAt`, `UpdatedAt`) automatically on create/update in server time (UTC used in code).

Examples (curl)

1) List all launches (includes related `Location`):

```bash
curl -sS https://localhost:5001/api/Launch
```

2) Get a single launch:

```bash
curl -sS https://localhost:5001/api/Launch/launch-123
```

3) Create a launch with a new nested location:

```bash
curl -X POST https://localhost:5001/api/Launch \
  -H "Content-Type: application/json" \
  -d '{
    "launchId":"launch-123",
    "rocketName":"Falcon 9",
    "date":"2026-01-07T12:00:00Z",
    "status":"SCHEDULED",
    "location": {
      "locationId": 0,
      "countryName":"USA",
      "latitude":"28.5623",
      "longitude":"-80.5772"
    }
  }'
```

4) Create a launch referencing an existing location (`locationId` must exist):

```bash
curl -X POST https://localhost:5001/api/Launch \
  -H "Content-Type: application/json" \
  -d '{
    "launchId":"launch-124",
    "rocketName":"Electron",
    "date":"2026-02-01T10:00:00Z",
    "status":"TBD",
    "locationId": 5
  }'
```

5) Update a launch:

```bash
curl -X PUT https://localhost:5001/api/Launch/launch-123 \
  -H "Content-Type: application/json" \
  -d '{
    "launchId":"launch-123",
    "rocketName":"Falcon 9 Block 5",
    "date":"2026-01-07T13:00:00Z",
    "status":"LAUNCHED",
    "location": { "locationId": 1 }
  }'
```

6) Delete a launch:

```bash
curl -X DELETE https://localhost:5001/api/Launch/launch-123
```

Examples (C# HttpClient)

```csharp
var client = new HttpClient { BaseAddress = new Uri("https://localhost:5001") };

// POST create
var launch = new {
  launchId = "launch-200",
  rocketName = "Demo",
  date = DateTime.UtcNow,
  status = "SCHEDULED",
  location = new { locationId = 0, countryName = "USA", latitude = "0", longitude = "0" }
};

var resp = await client.PostAsJsonAsync("/api/Launch", launch);
resp.EnsureSuccessStatusCode();

// GET list
var list = await client.GetFromJsonAsync<List<Launch>>("/api/Launch");
```

Good practices when using the endpoints
- Prefer DTOs on the client side to avoid sending internal-only fields.
- For listing endpoints, add server-side pagination if you expect many rows.
- Validate IDs and payloads before sending; controllers currently perform basic checks but do not enforce DTO validation.
- Run the server in Development to see detailed error messages; for production, use proper exception-handling middleware.

File location
- See this document: [RocketMoonApp.Server/CONTROLLERS_USAGE.md](RocketMoonApp.Server/CONTROLLERS_USAGE.md)

If you want, I can:
- Add DTOs and validation to the API.
- Implement the `IGenericRepository<T>` + service layer and refactor controllers to use it.
