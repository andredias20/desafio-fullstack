export interface RegisterByCityRequest {
  cityName: string
}

export interface RegisterByCoordinatesRequest {
  latitude: number
  longitude: number
}

export interface RegisterTemperatureResponse {
  temperatureCelsius: number
}

export interface TemperatureRecord {
  id: string
  cityName: string
  latitude: number
  longitude: number
  temperatureCelsius: number
  recordedAt: string
}
