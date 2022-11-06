void HTTP_init(void) {

  HTTP.on("/configs.json", handle_ConfigJSON); // формирование configs.json страницы для передачи данных в web интерфейс
  // API для устройства
  HTTP.on("/checkssid", handle_check_Ssid);     // Установить имя и пароль роутера по запросу вида /ssid?ssid=home2&password=12345678
  HTTP.on("/ssid", handle_set_Ssid);     // Установить имя и пароль роутера по запросу вида /ssid?ssid=home2&password=12345678
  HTTP.on("/login", handle_SaveLogin);
  // Запускаем HTTP сервер
  HTTP.begin();

}

// Функции API-Set
// Установка параметров для подключения к внешней AP по запросу вида http://192.168.0.101/ssid?ssid=home2&password=12345678
void handle_check_Ssid() {
  if(WiFiReload(HTTP.arg("ssid"), HTTP.arg("password"))){
    HTTP.send(200, "text/plain", "OK");   // отправляем ответ о выполнении
  } else {
    HTTP.send(400, "text/plain", "Not connected");
  }
}


void handle_set_Ssid() {
  if(_newssid.length()>0){
    _ssid = _newssid;
    _password = _newpass;
    _newssid = "";
    _newpass = "";
    saveConfig();
    ESP.restart(); 
  }
}

void handle_SaveLogin() {
  _login = HTTP.arg("login");
  saveConfig();
}

// Перезагрузка модуля по запросу вида http://192.168.0.101/restart?device=ok
void handle_Restart() {
  String restart = HTTP.arg("device");          // Получаем значение device из запроса
  if (restart == "ok") {                         // Если значение равно Ок
    HTTP.send(200, "text / plain", "Reset OK"); // Oтправляем ответ Reset OK
    ESP.restart();                                // перезагружаем модуль
  }
  else {                                        // иначе
    HTTP.send(200, "text / plain", "No Reset"); // Oтправляем ответ No Reset
  }
}

void handle_ConfigJSON() {
  String json = "{";  // Формировать строку для отправки в браузер json формат
  //{"SSDP":"SSDP-test","ssid":"home","password":"i12345678","ssidAP":"WiFi","passwordAP":"","ip":"192.168.0.101"}
  // Имя сети
  json += "\"ssid\":\"";
  json += _ssid;
  // Пароль сети
  json += "\",\"password\":\"";
  json += _password;
    // Имя сети
  json += "\",\"newssid\":\"";
  json += _newssid;
  // Пароль сети
  json += "\",\"newpassword\":\"";
  json += _newpass;
  //
  json += "\",\"login\":\"";
  json += _login;
  // IP устройства
  json += "\",\"ip\":\"";
  json += WiFi.localIP().toString();
  // MAC устройства
  json += "\",\"macaddr\":\"";
  json += WiFi.macAddress();
  // Подключение к сети
  json += "\",\"iswificonnect\":\"";
  json += (WiFi.status() == WL_CONNECTED);
  json += "\"}";
  HTTP.send(200, "text/json", json);
}
