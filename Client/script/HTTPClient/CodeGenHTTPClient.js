export class CodeGenHTTPClient {
  constructor() {
    this.xhr = new XMLHttpRequest();
  }

  send(url, data, method, callback) {
    this.xhr.open(method, url, true);

    this.xhr.onreadystatechange = function() {
      if (this.readyState === 4 && this.status === 200) {
        callback(this.responseText);
      }
    };

    if (method === "POST") {
      this.xhr.setRequestHeader("Content-Type", "application/json");
      this.xhr.send(JSON.stringify(data));
    } else {
      this.xhr.send();
    }
  }
}


