const proxy_conf = {
    "/api": {
      "target": "http://localhost:5170",
      "secure": false,
      "pathRewrite": {
        "^/api": "https://127.0.0.1:5170/api"
      }
    }
}

module.exports = proxy_conf


