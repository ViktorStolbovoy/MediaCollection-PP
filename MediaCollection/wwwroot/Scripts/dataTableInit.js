
function initDataTable(d, tableName, updateEndpoint, delSelector)
{

	d.deleted = [];
	d.editableOptions = {};
	d.features = {};
	d.features.aaSorting = [];
	d.features.paging = false;
	d.features.dom = 'rtif';
	d.features.ordering = true;

	d.add = function ()
	{
		d.dt.fnAddData($.extend(true, [],d.emptyData));
		d.dt.makeEditable(d.editableOptions);
	}

	d.del = function(element)
	{
		var rb = $(element || delSelector);
		if (rb.length > 0)
		{
			var id = rb.data("id") || rb.val();
			d.dt.fnDeleteRow(rb[0].parentNode.parentNode)

			if (id >= 0)
			{
				d.deleted.push(id);
			}
		}
	}

	d.save = function()
	{
		var post = {};
		post.Deleted = d.deleted;
		post.ParentId = d.parentId;
		var originals = {};
		for(var i = 0; i < d.initialData.length; i ++)
		{
			var item = d.initialData[i];
			originals[item[0]] = item;
		}
		post.Edits = [];
		var edits = post.Edits;
		var data = d.DT.data(); //No deleted rows here
		for(var i = 0; i < data.length; i ++)
		{
			var item = data[i];
			var changed = false;
			var orig = originals[item[0]];
			if (!orig) 
			{
				//New
				edits.push(item);
			} 
			else 
			{
				for(var cell = 1; cell < item.length; cell ++)
				{
					if (orig[cell] != item[cell]) 
					{
						edits.push(item);
						break;
					}
				}
			}
		}
		$.ajax(updateEndpoint, {"data":JSON.stringify(post), "type":"POST", "dataType":"json", "contentType": "application/json", "success": d.refresh});//Send
	}

	d.refresh = function (data, status)
	{
		d.deleted = [];
		if (!d.DT)
		{
			d.features.data = $.extend(true, [], d.initialData);
			var el = $('#' + tableName);
			d.DT = el.DataTable(d.features);
			d.dt = el.dataTable();
		}
		else
		{
			if (data) d.initialData = data;
			d.DT.clear();
			d.DT.rows.add($.extend(true, [], d.initialData));
			d.DT.draw();
		}
		d.dt.makeEditable(d.editableOptions);
	}

	d.fnOnBlur = function(sValue, settings) {
		var aPos = d.dt.fnGetPosition(this);
		d.dt.fnUpdate(sValue, aPos[0], aPos[1]);
	};

	$(document).ready(function () {
		d.refresh(null, null);
	});

}
