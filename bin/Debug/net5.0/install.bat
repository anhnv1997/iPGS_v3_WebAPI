sc create PGS_WEBAPI3 binPath= D:\2.KZTECH\5.DuAn\iParking_new\PGS_WEBAPI\PGS_WEBAPI\bin\Debug\net5.0
sc failure PGS_WEBAPI3 actions= restart/60000/restart/60000/""/60000 reset= 86400
sc start PGS_WEBAPI3
sc config PGS_WEBAPI3 start=auto