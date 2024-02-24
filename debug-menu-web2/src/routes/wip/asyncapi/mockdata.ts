export let asyncapiMockData = ` {
    "asyncapi": "2.6.0",
  "info": {
    "title": "Example AsyncAPI specification",
    "version": "0.1.0"
  },
  "channels": {
    "log": {
      "subscribe": {
        "tags": [
        {
          "name": "log"
        }
        ],
        "message": {
          "payload": {
            "type": "object",
            "properties": {
              "text": {
                "type": "string"
              },
              "details": {
                "type": "string"
              },
              "type": {
                "type": "string"
              }
            }
          }
        }
      }
    },
    "gameplay/spawn": {
      "publish": {
        "description": "asdf",
        "tags": [
        {
          "name": "button"
        }
        ],
        "message": {
          "payload": {
            "type": "object",
            "properties": {
              "exampleField": {
                "type": "string",
                "description": "This is an example text field"
              },
              "exampleNumber": {
                "type": "number"
              },
              "exampleDate": {
                "type": "string",
                "format": "date-time"
              }
            }
          }
        }
      }
    },
    "gameplay/restart": {
      "publish": {
        "tags": [
          {
            "name": "button"
          }
        ]
      }
    },
    "progression/level-up": {
      "publish": {
        "tags": [
        {
          "name": "button"
        }
        ]
      }
    },
    "progression/reset": {
      "publish": {
        "tags": [
        {
          "name": "button"
        }
        ]
      }
    },
    "progression/add-xp": {
      "publish": {
        "tags": [
        {
          "name": "button"
        }
        ]
      }
    },
    "progression/economy/add-gold": {
      "publish": {
        "tags": [
        {
          "name": "button"
        }
        ]
      }
    },
    "progression/economy/add-hc": {
      "publish": {
        "tags": [
        {
          "name": "button"
        }
        ]
      }
    },
    "progression/economy/reset": {
      "publish": {
        "tags": [
        {
          "name": "button"
        }
        ]
      }
    }
  }
  }`