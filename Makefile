BIN_DIR = bin/

serverapp: server/*.cs util/*.cs
	csc $^ -out:$(BIN_DIR)/$@

clientapp: client/*.cs util/*.cs
	csc $^ -out:$(BIN_DIR)/$@
