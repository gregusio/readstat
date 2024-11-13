package main

import (
    "fmt"
    "net/http"
    "github.com/rs/cors"
)

func main() {
    http.HandleFunc("/", func(w http.ResponseWriter, r *http.Request) {
        fmt.Fprintln(w, "Hello from Go backend!")
    })

    corsHandler := cors.New(cors.Options{
        AllowedOrigins: []string{"http://localhost:3000"},
        AllowedMethods: []string{"GET", "POST", "PUT", "DELETE"},
        AllowedHeaders: []string{"Content-Type"},
    })

    fmt.Println("Server running on http://localhost:8080")
    http.ListenAndServe(":8080", corsHandler.Handler(http.DefaultServeMux))
}
