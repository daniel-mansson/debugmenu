namespace DebugMenu.ExampleProducerClient;

public static class Mock {

public const string api1 = """"
                      {
                      "debugmenuapi": "1.0.0",
                      "channels": {
                        "log": {
                          "name": "Log",
                          "type": "log",
                          "subscribe": {
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
                        },
                        "gameplay/spawn": {
                          "name": "Spawn",
                          "category": "Commands",
                          "type": "button",
                          "settings": {
                            "color": "red"
                          },
                          "publish": {
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
                        },
                        "gameplay/restart": {
                          "type": "button",
                          "publish": {}
                        },
                        "progression/level-up": {
                          "type": "button",
                          "publish": {}
                        },
                        "progression/reset": {
                          "type": "button",
                          "publish": {}
                        },
                        "progression/add-xp": {
                          "type": "button",
                          "publish": {}
                        },
                        "progression/double-xp": {
                          "name": "Double XP",
                          "type": "toggle",
                          "publish": {
                            "type": "boolean"
                          },
                          "subscribe": {
                            "type": "boolean"
                          }
                        },
                        "progression/power-scale": {
                          "name": "Power Scale",
                          "type": "slider",
                          "publish": {
                            "type": "object",
                            "properties": {
                              "value": {
                                "type": "number",
                                "min": 10,
                                "max": 120,
                                "step": 0.1
                              }
                            }
                          },
                          "subscribe": {
                            "type": "object",
                            "properties": {
                              "value": {
                                "type": "number"
                              }
                            }
                          }
                        },
                        "progression/economy/add-gold": {
                          "type": "button",
                          "publish": {}
                        },
                        "progression/economy/add-hc": {
                          "type": "button",
                          "publish": {}
                        },
                        "progression/economy/reset": {
                          "type": "button",
                          "publish": {}
                        }
                      }
                    }
                    """";


public const string api2 = """"
                             {
                             "debugmenuapi": "1.0.0",
                             "channels": {
                               "log": {
                                 "name": "Log",
                                 "type": "log",
                                 "subscribe": {
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
                               },
                               "gameplay/spawn": {
                                 "name": "Spawn",
                                 "category": "Commands",
                                 "type": "button",
                                 "settings": {
                                   "color": "red"
                                 },
                                 "publish": {
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
                               },
                               "gameplay/restart": {
                                 "type": "button",
                                 "publish": {}
                               },
                               "progression/level-up": {
                                 "type": "button",
                                 "publish": {}
                               },
                               "progression/reset": {
                                 "type": "button",
                                 "publish": {}
                               },
                               "progression/add-xp": {
                                 "type": "button",
                                 "publish": {}
                               },
                           "progression/economy/add-gold": {
                             "type": "button",
                             "publish": {}
                           },
                           "progression/economy/add-hc": {
                             "type": "button",
                             "publish": {}
                           },
                           "progression/economy/reset": {
                             "type": "button",
                             "publish": {}
                           },
                           "progression/levels/unlock-next": {
                             "type": "button",
                             "publish": {}
                           },
                           "progression/levels/lock-all": {
                             "type": "button",
                             "publish": {}
                           },
                           "progression/levels/reset": {
                             "type": "button",
                             "publish": {}
                           }
                             }
                           }
                           """";
}
