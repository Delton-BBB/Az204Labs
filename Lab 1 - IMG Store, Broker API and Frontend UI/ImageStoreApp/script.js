// Base URL for the API
const baseUrl = "https://deltonwebapi.azurewebsites.net";

// Function to fetch data from an endpoint
function fetchEndpoint(endpoint) {
  const output = document.getElementById("output");
  output.textContent = "Loading...";

  fetch(`${baseUrl}/${endpoint}`)
    .then((response) => {
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      return response.json();
    })
    .then((data) => {
      output.textContent = JSON.stringify(data, null, 2);
    })
    .catch((error) => {
      output.textContent = `Error: ${error.message}`;
    });
}
