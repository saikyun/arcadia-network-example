(ns game.core
  (:require [arcadia.core :refer :all]
            [clojure.core.server :as s]
            [clojure.main :as m])
  (:import [UnityEngine.Networking NetworkManager NetworkClient NetworkServer]
           [UnityEngine Application]
           EdnMsg))

;; Uncomment this when building if you want a repl in the standalone game
;; Don't forget to comment it again before hitting "play" though
;; (s/start-server {:port 6015 :name "running-server" :accept `m/repl})

;; The ugly def is to make it easier to do stuff when you connect to the
;;   standalone games repl
;; This has to do with certain functions not being okay to run
;;   unless you're in the main thread. I couldn't get it to work so...
(defn client []
  (let [c (.. (cmpt (object-named "NetworkManager") NetworkManager) client)]
    (when c
      (def client-holder c))
    c))

;; If you wanna call this from the standalone repl, you can do something like
;;  (send-edn-to-server client-holder {:cool-data "yeah"})
(defn send-edn-to-server
  [client edn]
  (.. client (Send (.. EdnMsg MsgType) (EdnMsg. (prn-str edn)))))

(defn on-button-click []
  (send-edn-to-server (client) {:stuff "hej"}))

(defrole frame-rate-role
  (start [obj k]
         (set! (.. Application targetFrameRate) 5)))

(defonce data (atom {}))

(add-watch data :data-watcher (fn [_ _ _ _] (log "got new data! " @data)))

(defn is-server? [] (.. NetworkServer active))
(defn is-client? [] (.. NetworkClient active))

(defn got-edn
  [edn]
  (reset! data (read-string edn)))

(defn send-edn-to-all
  [edn]
  (.. NetworkServer (SendToAll (.. EdnMsg MsgType) (EdnMsg. (prn-str edn)))))

(defn send-resp
  [msg]
  (.. NetworkServer (SendToAll (.. EdnMsg MsgType) (EdnMsg. (prn-str msg)))))

(defrole button-role
  (on-pointer-click
   [obj click-data k]
   (on-button-click)))
