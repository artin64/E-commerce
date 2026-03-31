$ErrorActionPreference = "Stop"

$url = "http://localhost:5173/"

# Nëse s'ka instaluar node_modules, instalo automatikisht.
if (!(Test-Path "node_modules")) {
  Write-Host "Nuk u gjet `node_modules`. Po bej `npm install`..."
  npm install
}

# Hap shfletuesin (mund të duhet refresh nëse Vite s'ka mbaruar ende startin).
Start-Process $url

Write-Host "Po nis Vite..."
npm run dev

