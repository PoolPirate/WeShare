const PROXY_CONFIG = [
  {
    context: [
      "/Api",
   ],
    target: "http://localhost:8765",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
