<h1 align="center">Province-District-Ward API</h1>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-8-5C2D91?style=for-the-badge&logo=.net&logoColor=white" alt=".NET 8"/>
  <img src="https://img.shields.io/badge/Entity_Framework-512BD4?style=for-the-badge&logo=.net&logoColor=white" alt="Entity Framework"/>
</p>

<p>
This project uses .NET 8 and Entity Framework to build a RESTful API for managing provinces, districts, and wards in Vietnam. The CRUD operations are currently implemented without Authorization and Authentication, which can be added later if required for the project.
</p>

<h2>Public Endpoints</h2>

<p>The following public endpoints are available:</p>

<h3>1. Get all provinces:</h3>
<pre><code>GET https://{URL_Base}/province</code></pre>
<p>This endpoint returns a list of all provinces.</p>

<h3>2. Get districts by province code:</h3>
<pre><code>GET https://{URL_Base}/province/district/{province_code}</code></pre>
<p>This endpoint returns a list of districts within a specific province, identified by the province code.</p>

<h3>3. Get wards by district code:</h3>
<pre><code>GET https://{URL_Base}/province/ward/{district_code}</code></pre>
<p>This endpoint returns a list of wards within a specific district, identified by the district code.</p>

<hr>

