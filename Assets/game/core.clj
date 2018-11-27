(ns game.core
  (:require [arcadia.core :refer :all])
  (:import [UnityEngine.Networking NetworkServer]
           EdnMsg))

(defonce data (atom {}))

(add-watch data :data-watcher (fn [_ _ _ _] (log "got new data! " data)))

(defn got-edn
  [edn]
  (reset! data (read-string edn)))

(defn send-edn
  [edn]
  (.. NetworkServer (SendToAll (.. EdnMsg MsgType) (EdnMsg. edn))))
